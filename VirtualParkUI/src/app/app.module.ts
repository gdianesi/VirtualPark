import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HeaderComponent } from './layouts/header/header/header.component';
import { DropdownMenuComponent } from './components/dropdown-menu/dropdown-menu.component';
import { HomeComponent } from './pages/home/home.component';

@NgModule({
    declarations: [
        AppComponent,
    ],
    imports: [
        BrowserModule,
        AppRoutingModule,
        HttpClientModule,
        HeaderComponent,       
        HomeComponent,
        DropdownMenuComponent
    ],
    providers: [],
    bootstrap: [AppComponent]
})
export class AppModule {}