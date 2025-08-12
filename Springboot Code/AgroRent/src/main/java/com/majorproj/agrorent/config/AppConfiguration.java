package com.majorproj.agrorent.config;

import org.modelmapper.Conditions;
import org.modelmapper.ModelMapper;
import org.modelmapper.convention.MatchingStrategies;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.security.crypto.bcrypt.BCryptPasswordEncoder;
import org.springframework.security.crypto.password.PasswordEncoder;

import com.cloudinary.Cloudinary;
import com.cloudinary.utils.ObjectUtils;
import com.razorpay.RazorpayClient;
import com.razorpay.RazorpayException;

@Configuration
public class AppConfiguration {
	
	@Value("${cloudinary.cloud.name}")
	private String cloudName;
	@Value("${cloudinary.api.key}")
	private String apiKey;
	@Value("${cloudinary.api.secret}")
	private String apiSecret;
	
	@Value("${razorpay.api.key}")
	private String razorapiKey;
	
	@Value("${razorpay.api.secret}")
	private String razorapiSecret;
	
	
	
	@Bean 
	ModelMapper modelMapper() {
		
		ModelMapper mapper = new ModelMapper();
		/*
		 * to specify to ModelMapper - transfer only MATCHING props from src->dest
		 */
		mapper.getConfiguration().setMatchingStrategy(MatchingStrategies.STRICT)
				/*
				 * to specify to ModelMapper - do not transfer nulls
				 */
				.setPropertyCondition(Conditions.isNotNull());
		return mapper;

	}
	
	@Bean
	PasswordEncoder passwordEncoder() {
		return new BCryptPasswordEncoder();
	}
	
	@Bean
	Cloudinary cloudinary() {
		return new Cloudinary(ObjectUtils.asMap("cloud_name", cloudName, "api_key", apiKey, "api_secret", apiSecret)

		);
	}
	
	@Bean
    public RazorpayClient razorpayClient() throws RazorpayException {
        return new RazorpayClient(razorapiKey, razorapiSecret);
    }
	
}
