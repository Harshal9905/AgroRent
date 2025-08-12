package com.majorproj.agrorent.services;

import org.springframework.web.multipart.MultipartFile;

public interface ImageService {

	String uploadImage(MultipartFile image, String fileName);
	String getUrlFromPublicId(String publicId);
	void deleteImage(String publicId);
}
