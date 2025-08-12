package com.majorproj.agrorent.customeExceeption;

public class ApiException extends RuntimeException{
	public ApiException(String msg) {
		super(msg);
	}
}
