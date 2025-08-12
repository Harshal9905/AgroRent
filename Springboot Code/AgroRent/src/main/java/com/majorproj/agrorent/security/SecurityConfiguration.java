package com.majorproj.agrorent.security;

import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.http.HttpMethod;
import org.springframework.security.authentication.AuthenticationManager;
import org.springframework.security.config.Customizer;
import org.springframework.security.config.annotation.authentication.configuration.AuthenticationConfiguration;
import org.springframework.security.config.annotation.method.configuration.EnableMethodSecurity;
import org.springframework.security.config.annotation.web.builders.HttpSecurity;
import org.springframework.security.config.annotation.web.configuration.EnableWebSecurity;
import org.springframework.security.config.http.SessionCreationPolicy;
import org.springframework.security.web.SecurityFilterChain;
import org.springframework.security.web.authentication.UsernamePasswordAuthenticationFilter;

import lombok.AllArgsConstructor;

@Configuration
@EnableWebSecurity
@AllArgsConstructor
@EnableMethodSecurity
public class SecurityConfiguration {
	private final JwtCustomFilter jwtCustomFilter;
	
	@Bean
	SecurityFilterChain configureSecurityFilterChain(HttpSecurity http) throws Exception{
		http.csrf(csrf->csrf.disable());
		http.cors(Customizer.withDefaults());
		http.formLogin(form->form.disable());
		
//		http.exceptionHandling(exception->exception
//				.authenticationEntryPoint(authenticationEntryPoint)
//				.accessDeniedHandler(accessDeniedHandler)
//		);
		
		http.authorizeHttpRequests(request -> request
				.requestMatchers("/v*/api-docs/**", "/swagger-ui/**","/swagger-ui.html","/auth/**")
				.permitAll()
				.anyRequest().authenticated())
				.sessionManagement(session -> session.sessionCreationPolicy(SessionCreationPolicy.STATELESS));
		
		http.addFilterBefore(jwtCustomFilter, UsernamePasswordAuthenticationFilter.class);
		
		return http.build();
	}
	
	@Bean 
	AuthenticationManager authenticationManager
	(AuthenticationConfiguration config) throws Exception {
		return config.getAuthenticationManager();
	}
}
