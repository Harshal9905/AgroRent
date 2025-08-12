package com.majorproj.agrorent.controllers;

import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.core.Authentication;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import com.majorproj.agrorent.dto.PaymentVerificationRequestDto;
import com.majorproj.agrorent.services.PaymentService;

import jakarta.validation.Valid;
import lombok.AllArgsConstructor;

@RestController
@RequestMapping("/payment")
@AllArgsConstructor
public class PaymentController {
	
	private final PaymentService paymentService;
	
	@PostMapping("/create-payment/{bookingId}")
	public ResponseEntity<?> createPayment(@PathVariable Long bookingId,Authentication authentication){
		String email=authentication.getName();
		return ResponseEntity.status(HttpStatus.CREATED).body(paymentService.createPayment(bookingId,email));
		
	}
	
	@PostMapping("/verify")
	public ResponseEntity<?> verifyPayment(@Valid @RequestBody PaymentVerificationRequestDto dto){
		return ResponseEntity.ok(paymentService.verifyPayment(dto));
	}
	
	@GetMapping("/Equipments/payments")
	public ResponseEntity<?> verifyPayment(Authentication authentication){
		String email=authentication.getName();
		return ResponseEntity.ok(paymentService.myEquipmentPayments(email));
	}
	
	
}
