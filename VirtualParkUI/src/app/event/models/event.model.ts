import { AttractionModel } from '../../../backend/repositories/attraction-api-repository';

export interface EventModel {
  id: string;
  name: string;
  date: string; 
  capacity: string;
  cost: string;
  attractions?: (string | AttractionModel)[];
}

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