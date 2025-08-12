package com.majorproj.agrorent.services;

import java.time.LocalDateTime;
import java.util.UUID;

import org.modelmapper.ModelMapper;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import com.majorproj.agrorent.customeExceeption.ApiException;
import com.majorproj.agrorent.dao.FarmerDao;
import com.majorproj.agrorent.dao.VerificationTokenDao;
import com.majorproj.agrorent.dto.ApiResponse;
import com.majorproj.agrorent.dto.UserSignUpDto;
import com.majorproj.agrorent.entities.Farmer;
import com.majorproj.agrorent.entities.Role;
import com.majorproj.agrorent.entities.VerificationToken;

import lombok.AllArgsConstructor;


@Service
@Transactional
@AllArgsConstructor
public class AuthServiceImpl implements AuthService {

    private final PasswordEncoder passwordEncoder;
	
	private final FarmerDao farmerDao;
	private final ModelMapper mapper;
	private final EmailService emailService;
	private final VerificationTokenDao verificationTokenDao;
 

	@Override
	public ApiResponse userSignUp(UserSignUpDto dto) {
		if(farmerDao.existsByEmail(dto.getEmail())) {
			throw new ApiException("Email already exist!!");
		}
		
		Farmer farmer=mapper.map(dto, Farmer.class);
		farmer.setRole(Role.ROLE_FARMER);
		farmer.setPassword(passwordEncoder.encode(dto.getPassword()));
		farmer.setActive(false);
		farmerDao.save(farmer);
		
		String token=UUID.randomUUID().toString();
		VerificationToken verificationToken=new VerificationToken(token,farmer,LocalDateTime.now().plusDays(1));
		verificationTokenDao.save(verificationToken);
		
		String link="http://localhost:9090/auth/verify?token="+token;
		String subject="Activate your AgroRent Account";
		String message = """
		        Dear %s,

		        Thank you for signing up with AgroRent.
		        Please click the link below to activate your account:

		        %s

		        The link will expire in 24 hours.

		        Regards,
		        AgroRent Team
		        """.formatted(farmer.getFirstName()+" "+farmer.getLastName(), link);
		
		emailService.sendEmail(dto.getEmail(), subject, message);
		
		return new ApiResponse("Registration Successfull. Please check email to activate you account");
	}


	@Override
	public ApiResponse verifyToken(String token) {
		VerificationToken verifiedToken=verificationTokenDao.findByToken(token).orElseThrow(()->new ApiException("Invalid verification token"));
		
		Farmer farmer=verifiedToken.getFarmer();
		
		if(verifiedToken.getExpiryDate().isBefore(LocalDateTime.now())) {
			String newToken=UUID.randomUUID().toString();
			VerificationToken verificationToken=new VerificationToken(newToken,farmer,LocalDateTime.now().plusDays(1));
			verificationTokenDao.save(verificationToken);
			
			String link="http://localhost:9090/auth/verify?token="+newToken;
			String subject="Activate your AgroRent Account";
			String message = """
			        Dear %s,

			        Thank you for signing up with AgroRent.
			        Please click the link below to activate your account:

			        %s

			        The link will expire in 24 hours.

			        Regards,
			        AgroRent Team
			        """.formatted(farmer.getFirstName()+" "+farmer.getLastName(), link);
			
			emailService.sendEmail(farmer.getEmail(), subject, message);
			
			throw new ApiException("token expired");
		}
		
		farmer.setActive(true);
		farmerDao.save(farmer);
		verificationTokenDao.delete(verifiedToken);
		return new ApiResponse("Account successfully activated!");
	}

}
