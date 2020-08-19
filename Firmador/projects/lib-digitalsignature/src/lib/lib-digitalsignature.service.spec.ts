import { TestBed } from '@angular/core/testing';

import { DigitalSignatureService } from './lib-digitalsignature.service';

describe('DigitalSignatureService', () => {
  let service: DigitalSignatureService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DigitalSignatureService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
