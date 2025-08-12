package com.majorproj.agrorent.controllers;

import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.core.Authentication;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PatchMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.PutMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import com.majorproj.agrorent.dto.BookingReqDto;
import com.majorproj.agrorent.services.BookingService;

import lombok.AllArgsConstructor;
import lombok.extern.slf4j.Slf4j;

@RestController
@RequestMapping("/booking")
@AllArgsConstructor
@Slf4j
public class BookingController {
	private final BookingService bookingService;
	
	@PostMapping
	public ResponseEntity<?> createNewBooikng(@RequestBody BookingReqDto dto,Authentication authentication){
		log.info("id received ctrl"+dto.getEquipmentId());
		String email=authentication.getName();
		return ResponseEntity.status(HttpStatus.CREATED).body(bookingService.addBooking(email,dto));
	}
	
	@PatchMapping("/{id}/reject")
	public ResponseEntity<?> rejectBooking(@PathVariable Long id){
		log.info("In rejectBooking");
		return ResponseEntity.ok(bookingService.rejectBooking(id));
	}
	
	@PatchMapping("/{id}/accept")
	public ResponseEntity<?> acceptBooking(@PathVariable Long id){
		return ResponseEntity.ok(bookingService.acceptBooking(id));
	}
	
	@GetMapping("/owner") 
	public ResponseEntity<?> ownedEquipmentBooking(Authentication authentication){
		String email=authentication.getName();
		return ResponseEntity.ok(bookingService.getOwnedEquipmetsBooking(email));
	}
	
	@GetMapping("/my-bookings")
	public ResponseEntity<?> getUserBookings(Authentication authentication){
		String email=authentication.getName();
		return ResponseEntity.ok(bookingService.getFarmerBookings(email));
	}
	
	
}
