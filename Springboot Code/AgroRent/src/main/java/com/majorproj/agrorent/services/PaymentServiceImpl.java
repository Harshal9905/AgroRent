package com.majorproj.agrorent.services;

import java.time.LocalDateTime;
import java.util.List;
import java.util.Map;
import java.util.stream.Collectors;

import javax.security.sasl.AuthenticationException;

import org.json.JSONObject; 
import org.springframework.beans.factory.annotation.Value;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import com.majorproj.agrorent.customeExceeption.ApiException;
import com.majorproj.agrorent.dao.BookingDao;
import com.majorproj.agrorent.dao.PaymentDao;
import com.majorproj.agrorent.dto.ApiResponse;
import com.majorproj.agrorent.dto.PaymentDetailsDto;
import com.majorproj.agrorent.dto.PaymentRespDto;
import com.majorproj.agrorent.dto.PaymentVerificationRequestDto;
import com.majorproj.agrorent.entities.Booking;
import com.majorproj.agrorent.entities.BookingStatus;
import com.majorproj.agrorent.entities.Equipment;
import com.majorproj.agrorent.entities.Farmer;
import com.majorproj.agrorent.entities.Payment;
import com.majorproj.agrorent.entities.PaymentStatus;
import com.razorpay.Order;
import com.razorpay.RazorpayClient;
import com.razorpay.Utils;

import lombok.extern.slf4j.Slf4j;

@Service
@Transactional
@Slf4j
public class PaymentServiceImpl implements PaymentService {
	
	private final RazorpayClient razorpayClient;
	private final BookingDao bookingDao;
	private final PaymentDao paymentDao;
	private final EmailService emailService;
	
	@Value("${razorpay.api.secret}")
	private String razorapiSecret;
	
	

	public PaymentServiceImpl(RazorpayClient razorpayClient, BookingDao bookingDao, PaymentDao paymentDao,EmailService emailService) {
		super();
		this.razorpayClient = razorpayClient;
		this.bookingDao = bookingDao;
		this.paymentDao=paymentDao;
		this.emailService=emailService;
	}

	@Override
	public PaymentRespDto createPayment(Long bookingId, String email) {
		try {
			
			
			Booking booking=bookingDao.findById(bookingId).orElseThrow(()->new ApiException("Invalid Booking Id"));
			if(booking.getStatus() != BookingStatus.ACCEPTED) {
				throw new ApiException("Booking status is Invalid for payment");
			}
			Equipment equipment=booking.getEquipment();
			Farmer farmer=booking.getFarmer();
			
			if(!farmer.getEmail().equals(email)) {
				throw new AuthenticationException("Not authorized to make this payment");
			}
			
			int amountPaid=(int)(booking.getTotalAmount() * 100);
			
			JSONObject orderRequest=new JSONObject();
			orderRequest.put("amount", amountPaid);
	        orderRequest.put("currency", "INR");
	        orderRequest.put("receipt", "booking_" + bookingId);
			
	        Order order= razorpayClient.orders.create(orderRequest);
	        
	        Payment payment=new Payment();
	        payment.setOrderId(order.get("id"));
	        payment.setBooking(booking);
	        payment.setStatus(PaymentStatus.CREATED);
	        payment.setAmount(booking.getTotalAmount());
	        
	        paymentDao.save(payment);
	        
	        PaymentRespDto responseDto=new PaymentRespDto();
	        responseDto.setRazorpayOrderId(order.get("id"));
	        responseDto.setAmount(booking.getTotalAmount());
	        responseDto.setCurrency("INR");
	        responseDto.setName(farmer.getFirstName()+" "+farmer.getLastName());
	        responseDto.setEmail(farmer.getEmail());
	        responseDto.setEquipmentName(equipment.getName());
	        responseDto.setBookingId(bookingId);
	        
	        return responseDto;
	        
	        
		}catch (Exception e) {
			System.out.println(e.getMessage());
			throw new ApiException("Payment Creation failed. Try later");
		}
		
	}

	@Override
	public ApiResponse verifyPayment(PaymentVerificationRequestDto dto) {
		try {
			JSONObject attributes=new JSONObject(Map.of(
					"razorpay_signature",dto.getRazorpaySignature(),
					"razorpay_order_id",dto.getRazorpayOrderId(),
					"razorpay_payment_id",dto.getRazorpayPaymentId()
			));
					
			boolean isValid=Utils.verifyPaymentSignature(attributes, razorapiSecret);
			if(isValid) {
				Payment payment=paymentDao.findByBookingId(dto.getBookingId()).orElseThrow(()->new ApiException("Payment not found"));
				payment.setStatus(PaymentStatus.PAID);
				payment.setPaymentId(dto.getRazorpayPaymentId());
				payment.setTimestamp(LocalDateTime.now());
				paymentDao.save(payment);
				
				emailService.sendReceiptEmail(payment);
				return new ApiResponse("Payment verification successfull");
			}else {
				log.error("Invalid signature");
				throw new ApiException("Invalid signature!!");
			}
		}catch (Exception e) {
			log.error(e.getMessage());
			throw new ApiException("Payment verification failed!!");
		}
	}

	@Override
	public List<PaymentDetailsDto> myEquipmentPayments(String email) {
		List<Payment> payments=paymentDao.findByBookingEquipmentOwnerEmail(email);
		
		return payments.stream()
				.map(p-> new PaymentDetailsDto(
							p.getBooking().getId(),
							p.getBooking().getFarmer().getFirstName()+" "+p.getBooking().getFarmer().getLastName(),
							p.getBooking().getEquipment().getName(),
							p.getAmount(),
							p.getPaymentId(),
							p.getOrderId()
						))
				.collect(Collectors.toList());
	}
	
	

	
	
	
}

