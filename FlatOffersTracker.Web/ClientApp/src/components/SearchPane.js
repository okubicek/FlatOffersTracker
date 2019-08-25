import React, { Component } from 'react';
import { NumericBox } from './FormControls/NumericBox';
import { SelectBox } from './FormControls/SelectBox';

export class SearchPane extends Component {
    constructor(props) {
        super(props)

        this.state = {
            searchParams: {
                minFlatSize: '',
                roomCount: '',
                maxPrice: '',
                flatType: '',
            },
            roomCountOptions: [...new Set(props.items.map(item => item.numberOfRooms))],
            flatTypeOptions: [...new Set(props.items.map(item => item.flatType))]
        };

        this.handleParamsChange = this.handleParamsChange.bind(this);
        this.handleSearch = this.handleSearch.bind(this);
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

    render() {
        return (
            <div className = "row" >
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
                                <button type="submit"
                                    className="form-control"
                                    onClick={this.handleSearch}>Search</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        );
    }
}
