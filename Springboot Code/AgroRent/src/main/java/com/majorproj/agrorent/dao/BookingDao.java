package com.majorproj.agrorent.dao;

import java.time.LocalDate;
import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;

import com.majorproj.agrorent.entities.Booking;

public interface BookingDao extends JpaRepository<Booking, Long> {
	
	@Query("SELECT COUNT(b) > 0 FROM Booking b " +
		       "WHERE b.equipment.id = :equipmentId " +
		       "AND b.status <> 'CANCELLED' " + 
		       "AND b.status <> 'COMPLETED' " +
		       "AND b.status <> 'REJECTED' " +
		       "AND (" +
		       "     (b.startDate <= :endDate AND b.endDate >= :startDate)" +
		       ")")
	boolean existsBookingConflict(@Param("equipmentId") Long equipmentId,
		                              @Param("startDate") LocalDate startDate,
		                              @Param("endDate") LocalDate endDate);
	
	
	@Query("SELECT b FROM Booking b " +
		       "WHERE b.status = 'ACCEPTED' " +
		       "AND b.endDate < CURRENT_DATE")
	List<Booking> findBookingsToComplete();
	

	@Query("SELECT b FROM Booking b WHERE b.equipment.owner.email = :email")
    List<Booking> findByEquipmentOwnerEmail(@Param("email") String email);
	
	@Query("SELECT b FROM Booking b WHERE b.farmer.email = :email")
	List<Booking> findByFarmerEmail(@Param("email") String email);
}
