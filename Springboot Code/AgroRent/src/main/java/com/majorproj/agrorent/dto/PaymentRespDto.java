package com.majorproj.agrorent.dto;

import lombok.Getter;
import lombok.Setter;

@Getter
@Setter
public class PaymentRespDto {
	private String razorpayOrderId;
    private double amount;
    private String currency;
    private String name;
    private String email;
    private String equipmentName;
    private Long bookingId;

}
