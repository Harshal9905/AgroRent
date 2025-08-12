package com.majorproj.agrorent.dto;

import org.springframework.web.multipart.MultipartFile;

import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.constraints.Positive;
import lombok.Getter;
import lombok.Setter;

@Getter
@Setter
public class EquipmentDto {
	@NotBlank(message = "Name is required")
	private String name;
    private String description;
    @NotNull(message = "image is requires")
    private MultipartFile image;
    @NotNull(message = "rental amount is required")
    @Positive(message = "rentalprice must be positive")
    private double rentalPrice;
}
