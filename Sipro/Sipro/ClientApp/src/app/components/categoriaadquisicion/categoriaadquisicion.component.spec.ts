import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CategoriaadquisicionComponent } from './categoriaadquisicion.component';

describe('CategoriaadquisicionComponent', () => {
  let component: CategoriaadquisicionComponent;
  let fixture: ComponentFixture<CategoriaadquisicionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CategoriaadquisicionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CategoriaadquisicionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
