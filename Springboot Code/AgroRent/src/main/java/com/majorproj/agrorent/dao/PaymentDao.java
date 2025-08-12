package com.majorproj.agrorent.dao;

import java.util.List;
import java.util.Optional;

import org.springframework.data.jpa.repository.JpaRepository;

import com.majorproj.agrorent.entities.Payment;

public interface PaymentDao extends JpaRepository<Payment, Long> {
	
	Optional<Payment> findByBookingId(Long bookingId);
	
	List<Payment> findByBookingFarmerEmail(String email);
	
	List<Payment> findByBookingEquipmentOwnerEmail(String email);
}
