package com.majorproj.agrorent.dto;

import jakarta.validation.constraints.Email;
import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.Pattern;
import lombok.Getter;
import lombok.Setter;

@Getter
@Setter
public class UserSignUpDto {
	@NotBlank(message = "Firtname required")
	private String firstName;
	@NotBlank(message = "LastName required")
	private String lastName;
	@NotBlank(message = "Email required")
	@Email(message = "Inavlid Email")
    private String email;
    @Pattern(regexp = "((?=.*\\d)(?=.*[a-z])(?=.*[#@$*]).{5,20})", message = "invalid password format!!!!")
    private String password;
}
