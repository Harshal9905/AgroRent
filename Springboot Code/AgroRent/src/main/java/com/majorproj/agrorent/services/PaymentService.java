package com.majorproj.agrorent.services;

import java.util.List;

import com.majorproj.agrorent.dto.ApiResponse;
import com.majorproj.agrorent.dto.PaymentDetailsDto;
import com.majorproj.agrorent.dto.PaymentRespDto;
import com.majorproj.agrorent.dto.PaymentVerificationRequestDto;

public interface PaymentService {

	PaymentRespDto createPayment(Long bookingId, String email);
	
	ApiResponse verifyPayment(PaymentVerificationRequestDto dto);
	
	List<PaymentDetailsDto> myEquipmentPayments(String email);
	
}
