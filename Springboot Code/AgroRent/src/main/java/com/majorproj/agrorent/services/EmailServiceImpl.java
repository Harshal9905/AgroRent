package com.majorproj.agrorent.services;

import java.io.ByteArrayOutputStream;

import org.springframework.core.io.ByteArrayResource;
import org.springframework.mail.SimpleMailMessage;
import org.springframework.mail.javamail.JavaMailSender;
import org.springframework.mail.javamail.MimeMessageHelper;
import org.springframework.stereotype.Service;

import com.lowagie.text.*;
import com.lowagie.text.pdf.*;
import java.time.format.DateTimeFormatter;
import com.majorproj.agrorent.entities.Payment;

import jakarta.mail.internet.MimeMessage;
import lombok.AllArgsConstructor;

@Service
@AllArgsConstructor
public class EmailServiceImpl implements EmailService {
	
	private JavaMailSender mailSender;

	@Override
	public void sendEmail(String to, String subject, String body) {
		SimpleMailMessage message=new SimpleMailMessage();
		message.setFrom("atharvapimple30@gmail.com");
		message.setTo(to);
		message.setSubject(subject);
		message.setText(body);
		mailSender.send(message);
		
	}

	@Override
	public void sendReceiptEmail(Payment payment) {
		try {
            byte[] pdfData = generateReceiptPdf(payment);

            MimeMessage message = mailSender.createMimeMessage();
            MimeMessageHelper helper = new MimeMessageHelper(message, true);

            helper.setTo(payment.getBooking().getFarmer().getEmail());
            helper.setSubject("Your Payment Receipt - AgroRent");
            helper.setText("Dear " + payment.getBooking().getFarmer().getFirstName()+" "+payment.getBooking().getFarmer().getLastName() + ",\n\n" +
                    "Thank you for your payment. Please find the receipt attached.\n\n" +
                    "Regards,\nAgroRent Team");

            helper.addAttachment("Payment_Receipt.pdf", new ByteArrayResource(pdfData));

            mailSender.send(message);
        } catch (Exception e) {
            throw new RuntimeException("Failed to send email", e);
        }
		
	}
	
	 public byte[] generateReceiptPdf(Payment payment) {
	        try (ByteArrayOutputStream baos = new ByteArrayOutputStream()) {
	            Document document = new Document();
	            PdfWriter.getInstance(document, baos);
	            document.open();

	            Font titleFont = new Font(Font.HELVETICA, 18, Font.BOLD);
	            Font labelFont = new Font(Font.HELVETICA, 12, Font.BOLD);
	            Font normalFont = new Font(Font.HELVETICA, 12);

	            document.add(new Paragraph("AgroRent - Payment Receipt", titleFont));
	            document.add(new Paragraph(" "));
	            
	            PdfPTable table = new PdfPTable(2);
	            table.setWidthPercentage(100);

	            addRow(table, "Booking ID", String.valueOf(payment.getBooking().getId()), labelFont, normalFont);
	            addRow(table, "Farmer Name", payment.getBooking().getFarmer().getFirstName() + " " + payment.getBooking().getFarmer().getLastName(), labelFont, normalFont);
	            addRow(table, "Email", payment.getBooking().getFarmer().getEmail(), labelFont, normalFont);
	            addRow(table, "Equipment", payment.getBooking().getEquipment().getName(), labelFont, normalFont);
	            addRow(table, "Amount Paid", "₹" + String.format("%.2f", payment.getAmount()), labelFont, normalFont);
	            addRow(table, "Payment ID", payment.getPaymentId(), labelFont, normalFont);
	            addRow(table, "Order ID", payment.getOrderId(), labelFont, normalFont);
	            addRow(table, "Payment Date", payment.getTimestamp().format(DateTimeFormatter.ofPattern("dd-MM-yyyy HH:mm")), labelFont, normalFont);
	            addRow(table, "Status", payment.getStatus().toString(), labelFont, normalFont);

	            document.add(table);
	            document.close();

	            return baos.toByteArray();
	        } catch (Exception e) {
	            throw new RuntimeException("Error generating receipt PDF", e);
	        }
	    }

	    private void addRow(PdfPTable table, String label, String value, Font labelFont, Font valueFont) {
	        PdfPCell cell1 = new PdfPCell(new Phrase(label, labelFont));
	        PdfPCell cell2 = new PdfPCell(new Phrase(value, valueFont));
	        cell1.setPadding(5);
	        cell2.setPadding(5);
	        table.addCell(cell1);
	        table.addCell(cell2);
	    }

}
