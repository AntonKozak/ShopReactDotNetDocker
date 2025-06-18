'use server';

import { FieldValues } from 'react-hook-form';
import { fetchWrapper } from '../lib/fetchWrapper';
import { Auction, Bid, PagedResult } from '../types';

export async function getData(query: string): Promise<PagedResult<Auction>> {
  return fetchWrapper.get(`search${query}`);
}

export async function createAuction(data: FieldValues) {
  return fetchWrapper.post('auctions', data);
}

export async function getDetailedViewData(id: string): Promise<Auction> {
  return fetchWrapper.get(`auctions/${id}`);
}

export async function updateAuction(data: FieldValues, id: string) {
  return fetchWrapper.put(`auctions/${id}`, data);
}

export async function deleteAuction(id: string) {
  return fetchWrapper.del(`auctions/${id}`);
}

export async function getBidsForAuction(id: string): Promise<Bid[]> {
  return fetchWrapper.get(`bids/${id}`);
}

export async function placeBidForAuction(auctionId: string, amount: number) {
  return fetchWrapper.post(`bids?auctionId=${auctionId}&amount=${amount}`, {});
}
