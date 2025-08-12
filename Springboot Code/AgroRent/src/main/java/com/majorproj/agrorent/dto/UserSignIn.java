package com.majorproj.agrorent.dto;

import jakarta.validation.constraints.Email;
import jakarta.validation.constraints.NotBlank;
import lombok.Getter;
import lombok.Setter;

@Getter
@Setter
public class UserSignIn {
	@NotBlank(message = "Email required")
	@Email(message = "Inavlid Email")
	private String email;
	@NotBlank(message = "Password required")
	private String password; 
}
