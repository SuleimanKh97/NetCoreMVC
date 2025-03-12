import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CatagioryComponent } from './catagiory.component';

describe('CatagioryComponent', () => {
  let component: CatagioryComponent;
  let fixture: ComponentFixture<CatagioryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CatagioryComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CatagioryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
