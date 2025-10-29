import { CreateVisitorProfileRequest } from "./CreateVisitorProfileRequest";
export interface CreateUserRequest{
    name: string;
    lastName: string;
    email: string;
    password: string;
    rolesIds: string[],
    visitorProfile?: CreateVisitorProfileRequest;
}