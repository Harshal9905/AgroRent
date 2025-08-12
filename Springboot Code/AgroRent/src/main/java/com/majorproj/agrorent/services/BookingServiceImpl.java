package com.majorproj.agrorent.services;

import java.time.LocalDate;
import java.time.temporal.ChronoUnit;
import java.util.List;
import java.util.stream.Collectors;

import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import com.majorproj.agrorent.customeExceeption.ApiException;
import com.majorproj.agrorent.dao.BookingDao;
import com.majorproj.agrorent.dao.EquipmentDao;
import com.majorproj.agrorent.dao.FarmerDao;
import com.majorproj.agrorent.dto.ApiResponse;
import com.majorproj.agrorent.dto.BookingReqDto;
import com.majorproj.agrorent.dto.BookingResponseDTO;
import com.majorproj.agrorent.dto.BookingResponseOwnerDTO;
import com.majorproj.agrorent.entities.Booking;
import com.majorproj.agrorent.entities.BookingStatus;
import com.majorproj.agrorent.entities.Equipment;
import com.majorproj.agrorent.entities.Farmer;

import lombok.AllArgsConstructor;
import lombok.extern.slf4j.Slf4j;

@Service
@Transactional
@AllArgsConstructor
@Slf4j
public class BookingServiceImpl implements BookingService {
	private final BookingDao bookingDao;
	private final FarmerDao farmerDao;
	private final EquipmentDao equipmentDao;
	
	@Override
	public ApiResponse addBooking(String email, BookingReqDto dto) {
		log.info("id received:"+ dto.getEquipmentId());
		Farmer farmer=farmerDao.findByEmail(email).orElseThrow(()->new ApiException("Invalid Farmer"));
		Equipment equipment=equipmentDao.findById(dto.getEquipmentId()).orElseThrow(()->new ApiException("Invalid equipment id"));
		if (dto.getEndDate().isBefore(dto.getStartDate())) {
		    throw new ApiException("End date cannot be before start date.");
		}
		
		boolean isConflict=bookingDao.existsBookingConflict(dto.getEquipmentId(), dto.getStartDate(), dto.getEndDate());
		if(isConflict) {
			throw new ApiException("Equipment already booked for the selected dates!!");
		}
		
		Booking booking=new Booking();
		booking.setStartDate(dto.getStartDate());
		booking.setEndDate(dto.getEndDate());
		booking.setStatus(BookingStatus.PENDING);
		booking.setEquipment(equipment);
		equipment.addBooking(booking);
		booking.setFarmer(farmer);
		farmer.addBooking(booking);
		long totalDays=getDaysBetween(dto.getStartDate(), dto.getEndDate());
		booking.setTotalAmount(dto.getTotalAmount() * totalDays); 
		 
		bookingDao.save(booking);
		return new ApiResponse("Booking request submitted successfully.");
	}
	
	public static long getDaysBetween(LocalDate startDate, LocalDate endDate) {
        long days= ChronoUnit.DAYS.between(startDate, endDate);
        return days==0? 1: days;
    }

	@Override
	public ApiResponse rejectBooking(Long id) {
		Booking booking=bookingDao.findById(id).orElseThrow(()->new ApiException("Invalid booking id"));
		booking.setStatus(BookingStatus.REJECTED);
		return new ApiResponse("Booking request rejected");
	}

	@Override
	public ApiResponse acceptBooking(Long id) {
		Booking booking=bookingDao.findById(id).orElseThrow(()->new ApiException("Invalid booking id"));
		booking.setStatus(BookingStatus.ACCEPTED);
		return new ApiResponse("Booking request accepted");
	}

	@Override
	public List<BookingResponseOwnerDTO> getOwnedEquipmetsBooking(String email) {
		List<Booking> bookings = bookingDao.findByEquipmentOwnerEmail(email);

        return bookings.stream()
                .map(b -> new BookingResponseOwnerDTO(
                        b.getId(),
                        b.getStartDate(),
                        b.getEndDate(),
                        b.getStatus(),
                        b.getTotalAmount(),
                        isPaid(b),
                        b.getFarmer().getFirstName()+" "+b.getFarmer().getLastName(),
                        b.getEquipment().getName()
                )
                )
                .collect(Collectors.toList());
	}

	@Override
	public List<BookingResponseDTO> getFarmerBookings(String email) {
		List<Booking> bookings=bookingDao.findByFarmerEmail(email);
		return bookings.stream()
                .map(b -> new BookingResponseDTO(
                        b.getId(),
                        b.getStartDate(),
                        b.getEndDate(),
                        b.getStatus(),
                        b.getTotalAmount(),
                        isPaid(b),
                        b.getEquipment().getId(),
                        b.getEquipment().getName()
                )
                )
                .collect(Collectors.toList());
		
		
	}
	
	private boolean isPaid(Booking b) {
	    return b.getPayment() != null &&
	           b.getPayment().getPaymentId() != null &&
	           !b.getPayment().getPaymentId().isBlank();
	}
	
	
}
