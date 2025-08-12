package com.majorproj.agrorent.dto;

import java.time.LocalDate;

import com.majorproj.agrorent.entities.BookingStatus;

import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

@Data
@NoArgsConstructor
@AllArgsConstructor
public class BookingResponseOwnerDTO {
    private Long id;
    private LocalDate startDate;
    private LocalDate endDate;
    private BookingStatus status;
    private double totalAmount;
    private boolean isPaid;
    private String renterName;
    private String eqipmentName;
}