import React, { useCallback, useState } from "react"
import { Meta, StoryObj } from "@storybook/react"
import { userEvent, within, expect, waitFor, screen } from '@storybook/test';

import { DataTable } from "@/components/table/data-table"
import { PaginatedResponse } from "@/services"

type TestDto = {
    name: string
    email: string
    age: number
}

const testColumns = [
    { field: "name", key: "COLUMN_NAME_NAME", type: "string", sortable: true },
    { field: "email", key: "COLUMN_NAME_EMAIL", type: "string", sortable: true },
    { field: "age", key: "COLUMN_NAME_AGE", type: "number", sortable: true },
]

const originalTestData: PaginatedResponse<TestDto> = {
    links: {
        prev: { href: "#", method: "GET", rel: "prev" },
        next: { href: "#", method: "GET", rel: "next" },
        self: { href: "#", method: "GET", rel: "self" },
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
        columns: testColumns,
    },
}

const meta: Meta = {
    component: DataTable,
    parameters: {
        layout: "fullscreen",
    },
}

export default meta

function filterTestData(
    filters: Record<string, { operator: string; value: string }[]>
): PaginatedResponse<TestDto> {
    let filtered = [...(originalTestData.data ?? [])]
    Object.entries(filters).forEach(([field, operations]) => {
        operations.forEach(({ operator, value }) => {
            const fieldKey = field as keyof TestDto
            if (operator === "contains") {
                filtered = filtered.filter((item) =>
                    item.attributes[fieldKey]
                        ?.toString()
                        .toLowerCase()
                        .includes(value.toLowerCase())
                )
            }
            if (operator === "equals") {
                filtered = filtered.filter(
                    (item) =>
                        item.attributes[fieldKey]?.toString().toLowerCase() ===
                        value.toLowerCase()
                )
            }
        })
    })

    return {
        ...originalTestData,
        data: filtered,
    }
}


export const Default: StoryObj = {
    render: () => {
        const [filteredData, setFilteredData] = useState(originalTestData)

        const handleSearch = useCallback((term: string) => {
            const field = "name"
            setFilteredData(
                filterTestData({ [field]: [{ operator: "contains", value: term }] })
            )
        }, [])

        const handleFilter = useCallback(
            (filters: Record<string, { operator: string; value: string }[]>) =>
                setFilteredData(filterTestData(filters)),
            []
        )

        return (
            <DataTable<TestDto>>
                <DataTable.Content data={filteredData} onFilter={handleFilter}>
                    <DataTable.Toolbar onSearch={handleSearch} />
                    <DataTable.Container>
                        <DataTable.Header />
                        <DataTable.Body />
                    </DataTable.Container>
                </DataTable.Content>
                <DataTable.Pagination />
            </DataTable>
        )
    },
}

export const Empty: StoryObj = {
    render: () => {
        const emptyData: PaginatedResponse<TestDto> = {
            ...originalTestData,
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
                <DataTable.Pagination />
            </DataTable>
        )
    },
}

export const Loading: StoryObj = {
    render: () => {
        return (
            <DataTable<TestDto>>
                <DataTable.Content data={null}>
                    <DataTable.Toolbar />
                    <DataTable.Container>
                        <DataTable.Header />
                        <DataTable.Body />
                    </DataTable.Container>
                </DataTable.Content>
                <DataTable.Pagination />
            </DataTable>
        )
    },
}

export const FilterTest: StoryObj = {
    ...Default,
    play: async ({ canvasElement }) => {
        const canvas = within(canvasElement)

        const columnHeader = await canvas.findByText("COLUMN_NAME_NAME")
        const filterButton = columnHeader.closest("th")?.querySelector("button")
        await userEvent.click(filterButton!)

        const filterMenuItem = await screen.findByText("Filter")
        await userEvent.hover(filterMenuItem)

        const valueInput = await screen.findByPlaceholderText("Value...")

        await userEvent.clear(valueInput)
        await userEvent.type(valueInput, "Jane")

        const applyButton = await screen.findByRole("button", { name: "Apply" })
        await waitFor(() => expect(applyButton).toBeVisible())
        await userEvent.click(applyButton)

        await waitFor(() => {
            expect(canvas.getByText("Jane Smith")).toBeInTheDocument()
            expect(canvas.queryByText("John Doe")).not.toBeInTheDocument()
        })
    },
}

FilterTest.parameters = {
    tags: ['test'],
}