$(function () {
    const pagedTables = [];

    function setupPagination(tableSelector, pagerSelector) {
        const $table = $(tableSelector);
        const $pager = $(pagerSelector);

        if (!$table.length || !$pager.length) {
            return;
        }

        const pageSize = parseInt($table.data("page-size"), 10) || 5;
        const state = {
            $table,
            $pager,
            pageSize,
            page: 1
        };

        function getVisibleRows() {
            return $table.find("tbody tr").filter(function () {
                return $(this).data("search-hidden") !== true;
            });
        }

        function renderPager(totalPages) {
            $pager.empty();

            if (totalPages <= 1) {
                return;
            }

            const $nav = $('<div class="pager-wrap"></div>');
            const $prev = $(`<button type="button" class="btn btn-outline-secondary btn-sm"${state.page === 1 ? " disabled" : ""}>Previous</button>`);
            const $info = $(`<span class="pager-info">Page ${state.page} of ${totalPages}</span>`);
            const $next = $(`<button type="button" class="btn btn-outline-secondary btn-sm"${state.page === totalPages ? " disabled" : ""}>Next</button>`);

            $prev.on("click", function () {
                if (state.page > 1) {
                    state.page--;
                    update();
                }
            });

            $next.on("click", function () {
                if (state.page < totalPages) {
                    state.page++;
                    update();
                }
            });

            $nav.append($prev, $info, $next);
            $pager.append($nav);
        }

        function update() {
            const $rows = getVisibleRows();
            const totalRows = $rows.length;
            const totalPages = Math.max(1, Math.ceil(totalRows / pageSize));

            if (state.page > totalPages) {
                state.page = totalPages;
            }

            $table.find("tbody tr").each(function () {
                const $row = $(this);
                if ($row.data("search-hidden") === true) {
                    $row.hide();
                }
            });

            $rows.hide();

            const start = (state.page - 1) * pageSize;
            $rows.slice(start, start + pageSize).show();

            renderPager(totalPages);
        }

        state.update = update;
        pagedTables.push(state);
        update();
    }

    $(".table-pager").each(function () {
        const target = $(this).data("target");
        setupPagination(target, this);
    });

    $(".table-search").on("input", function () {
        const query = $(this).val().toString().trim().toLowerCase();
        const target = $(this).data("target");

        $(`${target} tbody tr`).each(function () {
            const rowText = $(this).text().toLowerCase();
            $(this).data("search-hidden", rowText.indexOf(query) === -1);
        });

        const pagingState = pagedTables.find((table) => `#${table.$table.attr("id")}` === target);
        if (pagingState) {
            pagingState.page = 1;
            pagingState.update();
        }
    });

    $(".collapse").on("shown.bs.collapse", function () {
        this.scrollIntoView({ behavior: "smooth", block: "nearest" });
        $(this).find("input, select, textarea").first().trigger("focus");
    });
});
