package com.majorproj.agrorent.entities;

import java.time.LocalDateTime;

import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.EnumType;
import jakarta.persistence.Enumerated;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.OneToOne;
import jakarta.persistence.Table;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;
import lombok.ToString;

@Entity
@NoArgsConstructor
@Getter
@Setter
@Table(name="payments")
@ToString(callSuper = true,exclude = {"booking"})
public class Payment extends BaseEntity{
	@Id 
	@GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;
	
    private String paymentId;      // Razorpay payment ID
    private String orderId;        // Razorpay order ID
    @Enumerated(EnumType.STRING)
    private PaymentStatus status;         // CREATED, PAID, FAILED
    @Column(nullable = false)
    private double amount;
    private LocalDateTime timestamp; 

    @OneToOne
    private Booking booking;
}
