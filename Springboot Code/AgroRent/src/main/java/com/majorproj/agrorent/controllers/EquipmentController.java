package com.majorproj.agrorent.controllers;


import org.springframework.http.HttpStatus;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.security.core.Authentication;
import org.springframework.web.bind.annotation.DeleteMapping;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.ModelAttribute;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.PutMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import com.majorproj.agrorent.dto.EquipmentDto;
import com.majorproj.agrorent.dto.EquipmentUpdateDto;
import com.majorproj.agrorent.services.EquipmentService;

import jakarta.validation.Valid;
import lombok.AllArgsConstructor;

@RestController
@RequestMapping("/equipment")
@AllArgsConstructor
public class EquipmentController {
	
	private final EquipmentService equipmentService;
	
	@PostMapping(consumes = MediaType.MULTIPART_FORM_DATA_VALUE)
	public ResponseEntity<?> addEquipment(@Valid @ModelAttribute EquipmentDto dto,Authentication authentication){
		String email=authentication.getName();
		
		return ResponseEntity.status(HttpStatus.CREATED).body(equipmentService.addEquipment(dto,email));
	}
	
	@GetMapping
	public ResponseEntity<?> getAllEquiments(Authentication authentication){
		String email=authentication.getName();
		return ResponseEntity.ok(equipmentService.getAllEquipments(email));
	}
	
	@PutMapping("/{equipmentId}")
	public ResponseEntity<?> updateEquipment(@PathVariable Long equipmentId, @Valid @ModelAttribute EquipmentUpdateDto dto){
		return ResponseEntity.ok(equipmentService.updateEquipment(equipmentId, dto));
		
	}
	
	@DeleteMapping("/{equipmentId}")
	@PreAuthorize("hasRole('ADMIN')")
	public ResponseEntity<?> deleteEquipment(@PathVariable Long equipmentId){
		return ResponseEntity.ok(equipmentService.deleteEquipment(equipmentId));
	}
	
	@GetMapping("/{equipmentId}")
	public ResponseEntity<?> getEquipment(@PathVariable Long equipmentId){
		return ResponseEntity.ok(equipmentService.getEquipmentById(equipmentId));
	}
	
	@GetMapping("/owned")
	public ResponseEntity<?> getUserEquipments(Authentication authentication){
		String email=authentication.getName();
		return ResponseEntity.ok(equipmentService.getUsersEquipment(email));
	}
	
	@DeleteMapping("/delete")
	public ResponseEntity<?> deleteMyEquipmnet(Authentication authentication){
		String email=authentication.getName(); 
		return ResponseEntity.ok(equipmentService.deleteEquipment(email));
	}
	
	
	
	
}
