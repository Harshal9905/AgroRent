package com.majorproj.agrorent.dao;

import java.util.Optional;

import org.springframework.data.jpa.repository.JpaRepository;

import com.majorproj.agrorent.entities.VerificationToken;

public interface VerificationTokenDao extends JpaRepository<VerificationToken, Long>{
	Optional<VerificationToken> findByToken(String token);
}
