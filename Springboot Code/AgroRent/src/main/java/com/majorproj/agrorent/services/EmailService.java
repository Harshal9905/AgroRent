package com.majorproj.agrorent.services;

import com.majorproj.agrorent.entities.Payment;

public interface EmailService {
	public void sendEmail(String to,String subject, String body);
	 void sendReceiptEmail(Payment payment);
}
