package com.majorproj.agrorent.dao;

import java.util.Optional;

import org.springframework.data.jpa.repository.JpaRepository;

import com.majorproj.agrorent.entities.Farmer;


public interface FarmerDao extends JpaRepository<Farmer, Long> {
	boolean existsByEmail(String email);
	
	Optional<Farmer> findByEmail(String email);
	        
}
