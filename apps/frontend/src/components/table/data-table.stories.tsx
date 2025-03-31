import React, { useCallback, useState } from "react"
import { Meta, StoryObj } from "@storybook/react"
import { userEvent, within, expect, waitFor, screen, fn } from '@storybook/test';

import { DataTable } from "@/components/table/data-table"
import { PaginatedResponse } from "@/services"

type TestDto = {
    name: string
    email: string
    age: number
}

const handleFilter = fn();
const handleSearch = fn();
const onPaginate = fn();

const testColumns = [
    { field: "name", key: "COLUMN_NAME_NAME", type: "string", sortable: true },
    { field: "email", key: "COLUMN_NAME_EMAIL", type: "string", sortable: true },
    { field: "age", key: "COLUMN_NAME_AGE", type: "number", sortable: true },
]

const data: PaginatedResponse<TestDto> = {
    links: {
        next: { href: "/next", method: "GET", rel: "next" },
        self: { href: "/self", method: "GET", rel: "self" },
    },
    data: [
        {
            type: "test",
            id: "1",
            attributes: { name: "John Doe", email: "john@example.com", age: 30 },
            links: { self: "#" },
        },
        {
            type: "test",
            id: "2",
            attributes: { name: "Jane Smith", email: "jane@example.com", age: 25 },
            links: { self: "#" },
        },
    ],
    metadata: {
        columns: testColumns
    }
}

const meta: Meta = {
    component: DataTable,
    parameters: {
        layout: "fullscreen",
    },
    tags: ['autodocs'],
}

export default meta


export const Default: StoryObj = {
    tags: ['default'],
    render: () => {
        return (
            <DataTable<TestDto>>
                <DataTable.Content data={data} onFilter={handleFilter}>
                    <DataTable.Toolbar onSearch={handleSearch} />
                    <DataTable.Container>
                        <DataTable.Header />
                        <DataTable.Body />
                    </DataTable.Container>
                </DataTable.Content>
                <DataTable.Pagination onPaginate={onPaginate} />
            </DataTable>
        )
    },
}

export const ExpandableRows: StoryObj = {
    tags: ['test'],
    render: () => {
        const expandableData: PaginatedResponse<TestDto> = {
            ...data,
            data: [
                {
                    type: "test",
                    id: "1",
                    attributes: {
                        name: "John Doe",
                        email: "john@example.com",
                        age: 30,
                        // Add nested data to show in expanded row
                        details: [
                            {
                                type: "test",
                                id: "1",
                                attributes: {
                                    address: "123 Main St",
                                    phone: "555-1234",
                                    preferences: ["coding", "reading"]
                                },
                                links: { self: "#" },
                            },
                            {
                                type: "test",
                                id: "2",
                                attributes: {
                                    address: "127 Main St",
                                    phone: "555-1234",
                                    preferences: ["coding", "reading"]
                                },
                                links: { self: "#" },
                            },
                            {
                                type: "test",
                                id: "3",
                                attributes: {
                                    address: "126 Main St",
                                    phone: "555-1234",
                                    preferences: ["coding", "reading"]
                                },
                                links: { self: "#" },
                            },
                            {
                                type: "test",
                                id: "4",
                                attributes: {
                                    address: "125 Main St",
                                    phone: "555-1234",
                                    preferences: ["coding", "reading"]
                                },
                                links: { self: "#" },
                            },
                            {
                                type: "test",
                                id: "5",
                                attributes: {
                                    address: "124 Main St",
                                    phone: "555-1234",
                                    preferences: ["coding", "reading"]
                                },
                                links: { self: "#" },
                            }
                        ]
                    },
                    links: { self: "#" },
                },
                {
                    type: "test",
                    id: "3",
                    attributes: {
                        name: "Bob Johnson",
                        email: "bob@example.com",
                        age: 42,
                        details: []
                    },
                    links: { self: "#" },
                }
            ],
            metadata: {
                columns: testColumns,
                 // Add nested metadata for expandable rows
                expandable: {
                    details: {
                        columns: [
                            { field: "address", key: "Address", type: "text", sortable: true },
                            { field: "phone", key: "Phone", type: "text", sortable: true },
                            { field: "preferences", key: "Preferences", type: "array", sortable: false }
                        ]
                    }
                }
            }
        };

        return (
            <DataTable<TestDto>>
                <DataTable.Content data={expandableData} onFilter={handleFilter}>
                    <DataTable.Toolbar onSearch={handleSearch} />
                    <DataTable.Container>
                        <DataTable.Header />
                        <DataTable.Body />
                    </DataTable.Container>
                </DataTable.Content>
                <DataTable.Pagination onPaginate={onPaginate} />
            </DataTable>
        )
    },
    
    play: async ({ canvasElement, step }) => {
        const canvas = within(canvasElement);

        await step('Find and click the expand button for John Doe', async () => {
            await waitFor(() => expect(canvas.getByText('John Doe')).toBeInTheDocument());

            const johnRow = canvas.getByText('John Doe').closest('tr');

            const expandButton = johnRow?.querySelector('button');
            if (!expandButton) throw new Error('Expand button not found');
            await userEvent.click(expandButton);
        });

        await step('Verify the expanded content is visible', async () => {
            await waitFor(() => {
                expect(canvas.getByText(/123 Main St/)).toBeInTheDocument();
                expect(canvas.getByText(/555-1234/)).toBeInTheDocument();
            });
        });
    }
}

export const Empty: StoryObj = {
    tags: ['empty'],
    render: () => {
        const emptyData: PaginatedResponse<TestDto> = {
            ...data,
            links: {
                self: { href: "#", method: "GET", rel: "self" }
            },
            data: [],
        }

        return (
            <DataTable<TestDto>>
                <DataTable.Content data={emptyData}>
                    <DataTable.Toolbar />
                    <DataTable.Container>
                        <DataTable.Header />
                        <DataTable.Body />
                    </DataTable.Container>
                </DataTable.Content>
                <DataTable.Pagination onPaginate={onPaginate} />
            </DataTable>
        )
    },
}

export const Loading: StoryObj = {
    tags: ['loading'],
    render: () => {
        return (
            <DataTable<TestDto>>
                <DataTable.Content data={null} skeletonRowCount={2}>
                    <DataTable.Toolbar />
                    <DataTable.Container>
                        <DataTable.Header />
                        <DataTable.Body />
                    </DataTable.Container>
                </DataTable.Content>
                <DataTable.Pagination onPaginate={onPaginate} />
            </DataTable>
        )
    },
}

export const Filtered: StoryObj = {
    ...Default,
    tags: ['test', '!autodocs'],
    play: async ({ canvasElement, step }) => {
        const canvas = within(canvasElement)

        await step('Open filter menu', async () => {
            const columnHeader = await canvas.findByText('COLUMN_NAME_NAME')
            const filterButton = columnHeader.closest('th')?.querySelector('button')
            if (!filterButton) throw new Error('Filter button not found')
            await userEvent.click(filterButton)
        })

        await step('Hover over filter menu item', async () => {
            const filterMenuItem = await screen.findByText('Filter')
            await userEvent.hover(filterMenuItem)
        })

        await step('Fill in filter value', async () => {
            const valueInput = await screen.findByPlaceholderText('Value...')
            await userEvent.clear(valueInput)
            await userEvent.type(valueInput, 'Jane')
        })

        await step('Click Apply button', async () => {
            const applyButton = await screen.findByRole('button', { name: 'Apply' })
            await waitFor(() => expect(applyButton).toBeVisible())
            await userEvent.click(applyButton)
        })

        await step('Check if handleFilter is called correctly', async () => {
            await waitFor(() => {
                expect(handleFilter).toHaveBeenCalledWith({
                    name: [{ operator: 'contains', value: 'Jane' }],
                })
            })
        })
    }
}