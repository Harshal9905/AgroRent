package com.majorproj.agrorent.dto;

import jakarta.validation.constraints.NotNull;
import lombok.Getter;
import lombok.Setter;

@Getter
@Setter
public class PaymentVerificationRequestDto {
	@NotNull(message = "razorpay OrderId required")
	private String razorpayOrderId;
	@NotNull(message = "razorpay paymentId required")
    private String razorpayPaymentId;
	@NotNull(message = "razorpay signature required")
    private String razorpaySignature;
	@NotNull(message = "BookingId required")
    private Long bookingId;
}
