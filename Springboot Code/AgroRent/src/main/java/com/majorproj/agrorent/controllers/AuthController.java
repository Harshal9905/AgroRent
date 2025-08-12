package com.majorproj.agrorent.controllers;

import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.authentication.AuthenticationManager;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.core.Authentication;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import com.majorproj.agrorent.dto.AuthResponse;
import com.majorproj.agrorent.dto.UserSignIn;
import com.majorproj.agrorent.dto.UserSignUpDto;
import com.majorproj.agrorent.security.JwtUtils;
import com.majorproj.agrorent.services.AuthService;

import jakarta.validation.Valid;
import lombok.AllArgsConstructor;

@RestController
@RequestMapping("/auth")
@AllArgsConstructor
public class AuthController { 
	
	private final AuthService authService;
	private final AuthenticationManager authManager;
	private final JwtUtils jwtUtils;
	
	@PostMapping("/signUp")
	public ResponseEntity<?> userSignUp(@Valid @RequestBody UserSignUpDto dto) {
		
		return ResponseEntity.status(HttpStatus.CREATED).body(authService.userSignUp(dto));
	}
	
	@PostMapping("/signIn")
	public ResponseEntity<?> userSignIn(@Valid @RequestBody UserSignIn dto) {
		
		UsernamePasswordAuthenticationToken authToken=new UsernamePasswordAuthenticationToken(dto.getEmail(), dto.getPassword());
		
		Authentication successAuth= authManager.authenticate(authToken);
		
		return ResponseEntity.status(HttpStatus.OK).body(new AuthResponse("Login successful",jwtUtils.generateJwtToken(successAuth)));
		
	}
	
	@GetMapping("/verify")
	public ResponseEntity<?> verifyUserEmail(@RequestParam String token){
		return ResponseEntity.ok(authService.verifyToken(token));
		
	}
}
