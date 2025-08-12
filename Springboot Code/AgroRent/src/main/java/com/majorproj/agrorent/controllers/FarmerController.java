package com.majorproj.agrorent.controllers;

import org.springframework.http.ResponseEntity;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;
import com.majorproj.agrorent.services.FarmerService;

import lombok.AllArgsConstructor;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PatchMapping;
import org.springframework.web.bind.annotation.PathVariable;


@RestController
@RequestMapping("/farmer")
@AllArgsConstructor
public class FarmerController {
	private final FarmerService farmerService;
	
	@GetMapping("/users")
	@PreAuthorize("hasRole('ADMIN')")
	public ResponseEntity<?> getAllUsers(){
		return ResponseEntity.ok(farmerService.getAll());
	}
	
	@PatchMapping("/enable/{email}")
	@PreAuthorize("hasRole('ADMIN')")
	public ResponseEntity<?> enableAccount(@PathVariable String email){
		return ResponseEntity.ok(farmerService.enableFarmer(email));
	}
	
	@PatchMapping("/disable/{email}")
	@PreAuthorize("hasRole('ADMIN')")
	public ResponseEntity<?> disableAccount(@PathVariable String email){
		return ResponseEntity.ok(farmerService.disableFarmer(email));
	}
}
