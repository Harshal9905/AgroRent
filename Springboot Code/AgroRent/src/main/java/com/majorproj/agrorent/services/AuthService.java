package com.majorproj.agrorent.services;

import com.majorproj.agrorent.dto.ApiResponse;
import com.majorproj.agrorent.dto.UserSignUpDto;

import jakarta.validation.Valid;

public interface AuthService {

	ApiResponse userSignUp(UserSignUpDto dto);

	ApiResponse verifyToken(String token);

}
