package com.majorproj.agrorent.services;

import java.io.IOException;

import org.springframework.stereotype.Service;
import org.springframework.web.multipart.MultipartFile;

import com.cloudinary.Cloudinary;
import com.cloudinary.Transformation;
import com.cloudinary.utils.ObjectUtils;
import com.majorproj.agrorent.config.AppConstants;
import com.majorproj.agrorent.customeExceeption.ApiException;

import lombok.AllArgsConstructor;

@Service
@AllArgsConstructor
public class ImageServiceImpl implements ImageService{

	private Cloudinary cloudinary;
	
	@Override
	public String uploadImage(MultipartFile image,String fileName) {
		
		
		
		try {
			byte[] data=new byte[image.getInputStream().available()];
			image.getInputStream().read(data);
			cloudinary.uploader().upload(data, ObjectUtils.asMap(
					"public_id",fileName
					));
			
			return getUrlFromPublicId(fileName);
			
		} catch (IOException e) {
			throw new ApiException("Image Uploading issue");
		}
		
		
		
	}

	@Override
	public String getUrlFromPublicId(String publicId) {
		return cloudinary
				.url()
				.transformation(
						new Transformation<>()
						.width(AppConstants.IMAGE_WIDTH)
						.height(AppConstants.IMAGE_HEIGHT)
						.crop(AppConstants.IMAGE_CROP)
				)
				.generate(publicId);
	}

	@Override
	public void deleteImage(String publicId) {
		try {
	        cloudinary.uploader().destroy(publicId, ObjectUtils.emptyMap());
	    } catch (IOException e) {
	        throw new ApiException("Failed to delete image from cloud");
	    }
		
	}

}
