import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DesembolsotipoComponent } from './desembolsotipo.component';

describe('DesembolsotipoComponent', () => {
  let component: DesembolsotipoComponent;
  let fixture: ComponentFixture<DesembolsotipoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DesembolsotipoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DesembolsotipoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
