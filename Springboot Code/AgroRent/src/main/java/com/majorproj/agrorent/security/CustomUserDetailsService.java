package com.majorproj.agrorent.security;

import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.security.core.userdetails.UserDetailsService;
import org.springframework.security.core.userdetails.UsernameNotFoundException;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import com.majorproj.agrorent.dao.FarmerDao;
import com.majorproj.agrorent.entities.Farmer;

import lombok.AllArgsConstructor;

@Service
@Transactional
@AllArgsConstructor
public class CustomUserDetailsService implements UserDetailsService{
	private FarmerDao dao;
	
	@Override
	public UserDetails loadUserByUsername(String email) throws UsernameNotFoundException {
		// TODO Auto-generated method stub
		Farmer user=dao.findByEmail(email).orElseThrow(()-> new UsernameNotFoundException("Invalid Email"));
		return user;
	}

}
