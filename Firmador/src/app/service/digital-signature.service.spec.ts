import { TestBed } from '@angular/core/testing';

import { DigitalSignatureService } from './digital-signature.service';

describe('SearchDocumentService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: DigitalSignatureService = TestBed.get(DigitalSignatureService);
    expect(service).toBeTruthy();
  });
});
