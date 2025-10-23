import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './pages/home/home.component';
import { ButtonsComponent } from './components/buttons/buttons.component';
import { HeaderComponent } from './layouts/header/header/header.component';

@NgModule({
    declarations: [AppComponent, HomeComponent, ButtonsComponent, HeaderComponent],
    imports: [BrowserModule, AppRoutingModule],
    providers: [],
    bootstrap: [AppComponent]
})
export class AppModule {}
