package com.majorproj.agrorent.dto;

import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

@Getter
@Setter
@AllArgsConstructor
@NoArgsConstructor
public class FarmerResponseDto {
	 private String firstName;
	 private String lastName;
	 private String email;
	 private String role; 
	 private boolean active;
}
