package com.asc.registration.service.mapper;

import com.asc.common.core.domain.entity.Consent;
import com.asc.common.core.domain.value.enums.ConsentStatus;
import com.asc.registration.service.transfer.response.ClientInfoResponse;
import com.asc.registration.service.transfer.response.ConsentResponse;
import org.springframework.stereotype.Component;

/**
 * ConsentDataMapper is responsible for mapping consent domain objects to data transfer objects
 * (DTOs). This component handles the transformation logic for creating consent response DTOs.
 */
@Component
public class ConsentDataMapper {

  /**
   * Maps a Consent domain object and a ClientResponse DTO to a ConsentResponse DTO.
   *
   * @param consent The consent domain object.
   * @param client The client info response DTO associated with the consent.
   * @return The mapped ConsentResponse DTO.
   * @throws IllegalArgumentException if consent or client is null
   */
  public ConsentResponse toConsentResponse(Consent consent, ClientInfoResponse client) {
    if (consent == null) throw new IllegalArgumentException("Consent cannot be null");
    if (client == null) throw new IllegalArgumentException("ClientResponse cannot be null");

    return ConsentResponse.builder()
        .registeredClientId(consent.getId().getRegisteredClientId())
        .principalName(consent.getId().getPrincipalName())
        .scopes(String.join(",", consent.getScopes()))
        .invalidated(consent.getStatus().equals(ConsentStatus.INVALIDATED))
        .modifiedOn(consent.getModifiedOn())
        .client(client)
        .build();
  }
}
