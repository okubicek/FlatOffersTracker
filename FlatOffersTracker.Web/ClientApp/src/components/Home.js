import React, { Component, Fragment} from 'react';
import { FlatOffersOverview } from './FlatOffersOverview';
import { SearchPane } from './SearchPane';
import { SlidePane } from './SlidePane';

import './home.css';

export class Home extends Component {
    constructor(props) {
        super(props);

        this.fetchFlatOffers = this.fetchFlatOffers.bind(this);

        this.state = {
            flatOffers: [], loading: false, pageSize: 18, pageNumber: 1, searchParams: null
        };
    }

    componentDidMount() {
        this.fetchFlatOffers();
    }

    handleSearch(searchParams) {
        this.setState({ searchParams: searchParams, flatOffers: [], pageNumber: 1, isThereMorePagesToLoad: true },
            () => this.fetchFlatOffers());
    }

    handleShowMore() {
        this.setState({ pageNumber: this.state.pageNumber + 1 },
            () => this.fetchFlatOffers());
    }

    fetchFlatOffers() {
        if (this.state.loading == true) {
            return;
        }

        this.setState({ loading: true });

        var url = new URL("api/FlatOffers/Get", "https://" + window.location.host);

        var searchParams = this.state.searchParams;
        if (searchParams != null) {
            Object.keys(searchParams).forEach(key => url.searchParams.append(key, searchParams[key]));
        }

        url.searchParams.append('pageNumber', this.state.pageNumber)
        url.searchParams.append('pageSize', this.state.pageSize)

        fetch(url)
            .then(response => response.json())
            .then(data => {
                var offers = this.state.flatOffers.concat(data.results);
                var isThereMorePagesToLoad = this.state.pageNumber < data.pageCount;
                this.setState({
                    flatOffers: offers,
                    loading: false,
                    pageNumber: this.state.pageNumber,
                    isThereMorePagesToLoad: isThereMorePagesToLoad
                })
            });
    }



    static displayName = Home.name;

    render () {
        return (
            <React.Fragment>
                <div className="container fluid">
                    <SlidePane label='Search'>
                        <SearchPane searchParams={this.state.searchParams} handleSearch={this.handleSearch.bind(this)} />
                    </SlidePane>
                    <FlatOffersOverview
                        flatOffers={this.state.flatOffers}
                        loading={this.state.loading}
                        isThereMorePagesToLoad={this.state.isThereMorePagesToLoad}
                        handleShowMore={this.handleShowMore.bind(this)} />                    
                </div>
            </React.Fragment>
        );
    }
}
