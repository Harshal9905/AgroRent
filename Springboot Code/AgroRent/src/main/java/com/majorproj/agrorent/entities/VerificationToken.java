package com.majorproj.agrorent.entities;

import java.time.LocalDateTime;

import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.OneToOne;
import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

@Entity
@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
public class VerificationToken {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    private String token;

    @OneToOne
    private Farmer farmer;

    private LocalDateTime expiryDate;

	public VerificationToken(String token, Farmer farmer, LocalDateTime expiryDate) {
		this.token = token;
		this.farmer = farmer;
		this.expiryDate = expiryDate;
	}
    
    
}
