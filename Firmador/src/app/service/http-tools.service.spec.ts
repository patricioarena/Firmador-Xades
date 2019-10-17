import { TestBed } from '@angular/core/testing';

import { HttpToolsService } from './http-tools.service';

describe('HttpToolsService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: HttpToolsService = TestBed.get(HttpToolsService);
    expect(service).toBeTruthy();
  });
});
