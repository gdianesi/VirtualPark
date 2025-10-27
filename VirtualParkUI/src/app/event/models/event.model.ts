import { AttractionModel } from '../../../backend/repositories/attraction-api-repository';



export interface CreateEventRequest {
  name: string;
  date: string;
  capacity: number;
  cost: number;
  attractionIds: string[];
}

export interface CreateEventResponse {
  id: string;
}