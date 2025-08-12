package com.majorproj.agrorent.services;

import java.util.List;

import com.majorproj.agrorent.dto.ApiResponse;
import com.majorproj.agrorent.dto.BookingReqDto;
import com.majorproj.agrorent.dto.BookingResponseDTO;
import com.majorproj.agrorent.dto.BookingResponseOwnerDTO;

public interface BookingService {

	ApiResponse addBooking(String email, BookingReqDto dto);

	ApiResponse rejectBooking(Long id);
	
	ApiResponse acceptBooking(Long id);

	List<BookingResponseOwnerDTO> getOwnedEquipmetsBooking(String email);

	List<BookingResponseDTO> getFarmerBookings(String email);
	

}
