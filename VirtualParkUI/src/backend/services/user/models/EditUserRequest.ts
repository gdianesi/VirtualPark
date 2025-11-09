import { CreateVisitorProfileRequest } from "./CreateVisitorProfileRequest";
export interface EditUserRequest{
    name: string;
    lastName: string;
    email: string;
    rolesIds: string[],
    visitorProfile?: CreateVisitorProfileRequest;
}
