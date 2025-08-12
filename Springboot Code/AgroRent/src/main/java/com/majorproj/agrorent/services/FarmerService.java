package com.majorproj.agrorent.services;

import java.util.List;

import com.majorproj.agrorent.dto.ApiResponse;
import com.majorproj.agrorent.dto.FarmerResponseDto;
import com.majorproj.agrorent.entities.Farmer;

public interface FarmerService {
	Farmer getByEmail(String email);
	
	List<FarmerResponseDto> getAll();
	
	ApiResponse enableFarmer(String email);
	
	ApiResponse disableFarmer(String email);
}
