package com.majorproj.agrorent.services;

import java.util.List;
import java.util.stream.Collectors;

import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import com.majorproj.agrorent.customeExceeption.ApiException;
import com.majorproj.agrorent.dao.FarmerDao;
import com.majorproj.agrorent.dto.ApiResponse;
import com.majorproj.agrorent.dto.FarmerResponseDto;
import com.majorproj.agrorent.entities.Farmer;

import lombok.AllArgsConstructor;

@Service
@Transactional
@AllArgsConstructor
public class FarmerServiceImpl implements FarmerService {
	
	private final FarmerDao farmerDao;
	
	@Override
	public Farmer getByEmail(String email) {
		
		return farmerDao.findByEmail(email).orElseThrow(()->new ApiException("Inavlid Farmer email"));
	}

	@Override
	public List<FarmerResponseDto> getAll() {
		List<Farmer> farmers=farmerDao.findAll();
		
		return farmers.stream()
				.map(f-> new FarmerResponseDto(
						f.getFirstName(),
						f.getLastName(),
						f.getEmail(),
						f.getRole().name(),
						f.isEnabled()
					))
				.collect(Collectors.toList());
		
	}

	@Override
	public ApiResponse enableFarmer(String email) {
		Farmer farmer=farmerDao.findByEmail(email).orElseThrow(()->new ApiException("Invalid email"));
		farmer.setActive(true);
		farmerDao.save(farmer);
		return new ApiResponse("User account enabled");
	}

	@Override
	public ApiResponse disableFarmer(String email) {
		Farmer farmer=farmerDao.findByEmail(email).orElseThrow(()->new ApiException("Invalid email"));
		farmer.setActive(false);
		farmerDao.save(farmer);
		return new ApiResponse("User account disabled");
	}

}
