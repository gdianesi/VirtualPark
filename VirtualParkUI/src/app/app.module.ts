import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HeaderComponent } from './layouts/header/header/header.component';
import { DropdownMenuComponent } from './components/dropdown-menu/dropdown-menu.component';
import { HomeComponent } from './pages/home/home.component';
import { HttpClientModule } from '@angular/common/http';
import { UserLoginFormComponent } from './business-components/user-login-form/user-login-form.component';

@NgModule({
    declarations: [
        AppComponent,
        UserLoginFormComponent,
    ],
    imports: [
        BrowserModule,
        AppRoutingModule,
        HomeComponent,
        DropdownMenuComponent,
        HttpClientModule
    ],
    providers: [],
    bootstrap: [AppComponent]
})
export class AppModule {}