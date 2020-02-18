import React, { Component } from 'react';
import { NumericBox } from './FormControls/NumericBox';
import { SelectBox } from './FormControls/SelectBox';
import { DateBox } from './FormControls/DateBox';

export class SearchPane extends Component {
    constructor(props) {
        super(props)

        this.state = {
            searchParams: {
                minFlatSize: '',
                roomCount: '',
                maxPrice: '',
                flatType: '',
                startDate: new Date(),
                endDate: new Date()
            },
            roomCountOptions: [],
            flatTypeOptions: []
        };

        this.handleParamsChange = this.handleParamsChange.bind(this);
        this.handleSearch = this.handleSearch.bind(this);

        this.getDefinitions("FlatType")
            .then(response => response.json())
            .then(data => {
                this.setState({ flatTypeOptions: data.map((item) => ({ key: item.key, value : item.value })) })
            });

        this.getDefinitions("NumberOfRooms")
            .then(response => response.json())
            .then(data => {
                this.setState({ roomCountOptions: data.map((item) => ({ key: item.key, value : item.value }))})
            });
    }

    handleParamsChange(target) {
        var value = target.type === 'checkbox' ? target.checked : target.value;
        var name = target.name;

        var { searchParams } = { ...this.state };
        searchParams[name] = value;

        this.setState({ searchParams: searchParams });
    }

    handleSearch(target) {
        var searchParams = this.state.searchParams;

        this.props.handleSearch(searchParams);
    }

    getDefinitions(type) {
        var url = new URL("api/FlatOffers/Definitions", "https://" + window.location.host);
        url.searchParams.append("definitionType", type);

        return fetch(url)
    }

    render() {
        return (
            <div className="row">
                <div className="col">
                    <div className="card">
                        <div className="card-header">Search</div>
                        <div className="card-body">
                            <div className="row">
                                <div className="col col-sm-6">       
                                    <NumericBox label="Min Flat Size"
                                        name="minFlatSize"
                                        value={this.state.searchParams.minFlatSize}
                                        onChange={this.handleParamsChange}
                                    />
                                    <SelectBox
                                        label="Number of Rooms"
                                        name="roomCount"
                                        value={this.state.searchParams.roomCount}
                                        onChange={this.handleParamsChange}
                                        options={this.state.roomCountOptions}
                                    />
                                    <NumericBox label="Max Price"
                                        name="maxPrice"
                                        value={this.state.searchParams.maxPrice}
                                        onChange={this.handleParamsChange}
                                    />
                                </div>
                                <div className="col col-sm-6">
                                    <SelectBox
                                        label="Flat Type"
                                        name="flatType"
                                        value={this.state.searchParams.flatType}
                                        onChange={this.handleParamsChange}
                                        options={this.state.flatTypeOptions}
                                    />
                                    <DateBox
                                        label="Start Date"
                                        value={this.state.searchParams.startDate}
                                        onChange={this.handleParamsChange}
                                        name="startDate"
                                    />
                                    <DateBox
                                        label="End Date"
                                        value={this.state.searchParams.endDate}
                                        onChange={this.handleParamsChange}
                                        name="endDate"
                                    />
                                    <button type="submit"
                                        className="form-control btn-primary"
                                        onClick={this.handleSearch}>Search</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        );
    }
}
