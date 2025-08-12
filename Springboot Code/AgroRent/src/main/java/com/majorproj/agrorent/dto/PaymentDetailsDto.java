package com.majorproj.agrorent.dto;

import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
public class PaymentDetailsDto {
	Long bookingId;
	String renter;
	String equipmentName;
	Double amount;
	String paymentId;
	String orderId;
}
